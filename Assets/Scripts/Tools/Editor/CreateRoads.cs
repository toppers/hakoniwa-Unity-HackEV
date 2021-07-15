using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using Hakoniwa.Tools.RoadMap;

public class CreateRoads : EditorWindow
{
    private int index = 0;
    private GameObject parent;
    private RoadMap map;

    [MenuItem("Window/Create Other/Create Roads")]
    static void Init()
    {
        EditorWindow.GetWindow<CreateRoads>(true, "Create Roads");
    }
    void OnEnable()
    {
        if (Selection.gameObjects.Length > 0) parent = Selection.gameObjects[0];
    }
    void OnSelectionChange()
    {
        Repaint();
    }
    void OnGUI()
    {
        try
        {

            parent = EditorGUILayout.ObjectField("Parent", parent, typeof(GameObject), true) as GameObject;

            GUILayout.Label("", EditorStyles.boldLabel);
            if (GUILayout.Button("Create"))
            {
                Create();
            }
        }
        catch (System.FormatException) { }
    }

    private int GetInstanceAngleIndex(RoadEntryInstance e)
    {
        float rotation = 0;
        if (e.cfg_entry.rotation < 0)
        {
            rotation = 360.0f + e.cfg_entry.rotation;
        }
        else
        {
            rotation = e.cfg_entry.rotation;
        }
        //Debug.Log("rotation=" + rotation + " index=" +(int)rotation/90) ;
        return ((int)rotation / 90);
    }

    private int GetRelativeAngleIndex(RoadEntryInstance e, int cinx)
    {
        int instance_angle_index = GetInstanceAngleIndex(e);

        int inx = cinx - instance_angle_index;
        if (inx < 0)
        {
            inx += 4;
        }
        //Debug.Log("index=" + inx);
        return inx;
    }

    private float GetShiftSizeZ(RoadEntryInstance e, int cinx)
    {
        //Debug.Log("GetShiftSizeZ: cinx=" + cinx + "GetAngleIndex(e, cinx)=" + GetAngleIndex(e, cinx));
        int instance_angle_index = GetInstanceAngleIndex(e);
        int rinx = GetRelativeAngleIndex(e, cinx);
        //Debug.Log("GetShiftSizeZ: inx=" + inx);
        switch (instance_angle_index)
        {
            case 0:
                return +1.0f * e.parts_type.shift_size[rinx].z;
            case 1:
                return -1.0f * e.parts_type.shift_size[rinx].x;
            case 2:
                return -1.0f * e.parts_type.shift_size[rinx].z;
            case 3:
                return +1.0f * e.parts_type.shift_size[rinx].x;
            default:
                break;
        }
        return 0;
    }
    private float GetShiftSizeX(RoadEntryInstance e, int cinx)
    {
        int rinx = GetRelativeAngleIndex(e, cinx);
        int instance_angle_index = GetInstanceAngleIndex(e);
        //Debug.Log("GetShiftSizeZ: inx=" + inx);
        switch (instance_angle_index)
        {
            case 0:
                return +1.0f * e.parts_type.shift_size[rinx].x;
            case 1:
                return +1.0f * e.parts_type.shift_size[rinx].z;
            case 2:
                return -1.0f * e.parts_type.shift_size[rinx].x;
            case 3:
                return -1.0f * e.parts_type.shift_size[rinx].z;
            default:
                break;
        }
        return 0;
    }

    private int GetLocateIndex(RoadEntryInstance e)
    {
        int index = 0;
        if (e.cfg_entry.connect_direction.Contains("z"))
        {
            if (e.cfg_entry.connect_direction.Contains("-"))
            {
                index = 2;
            }
            else
            {
                index = 0;
            }
        }
        else
        {
            if (e.cfg_entry.connect_direction.Contains("-"))
            {
                index = 3;
            }
            else
            {
                index = 1;
            }
        }
        return index;
    }

    private void CalculatePos(RoadEntryInstance prev_e, RoadEntryInstance e)
    {
        //prev prefab name     instance_angle    x, z, can_locate
        //current prefab name  instance_angle    locate_angle
        float pos_z = prev_e.pos.z;
        float pos_x = prev_e.pos.x;
        float scale = 1.0f;
        int locate_index = GetLocateIndex(e);
        int rinx = GetRelativeAngleIndex(prev_e, locate_index);

        Debug.Log("LOCATION-INDEX: " + locate_index);

        if (prev_e.cfg_entry.scale > 0.0f)
        {
            scale = prev_e.cfg_entry.scale;
        }
        float prev_size_z = GetShiftSizeZ(prev_e, locate_index) * scale;
        float prev_size_x = GetShiftSizeX(prev_e, locate_index) * scale;
        Debug.Log("PREV: " + prev_e.prefab_fname + "angle: " + GetInstanceAngleIndex(prev_e)
            + " Z=" + prev_size_z + " X=" + prev_size_x + " RINX=" + rinx);

        if (!prev_e.parts_type.shift_size[rinx].can_locate)
        {
            throw new InvalidDataException("Invalid Location:CURR: " + e.prefab_fname + "angle: " + GetInstanceAngleIndex(e));
        }

        //current entry size
        scale = 1.0f;
        if (e.cfg_entry.scale > 0.0f)
        {
            scale = e.cfg_entry.scale;
        }
        float c_size_z = GetShiftSizeZ(e, locate_index) * scale;
        float c_size_x = GetShiftSizeX(e, locate_index) * scale;
        Debug.Log("CURR: " + e.prefab_fname + "angle: " + GetInstanceAngleIndex(e) 
            + " Z=" + c_size_z + " X=" + c_size_x);
        pos_z += (prev_size_z + c_size_z);
        pos_x += (prev_size_x + c_size_x);

        e.pos.z = pos_z;
        e.pos.x = pos_x;
        return;
    }
    private void Create()
    {
        this.index = 0;
        RoadEntryInstance e = null;
        RoadEntryInstance prev_e = null;
        Dictionary<string, RoadEntryInstance> hash = new Dictionary<string, RoadEntryInstance>();

        string jsonString = File.ReadAllText("./road_map.json");
        this.map = JsonConvert.DeserializeObject<RoadMap>(jsonString);

        jsonString = File.ReadAllText("./road_parts_type.json");
        RoadEntryInstance.parts = JsonConvert.DeserializeObject<RoadParts>(jsonString);

        foreach (var entry in this.map.entries)
        {
            var e_type = RoadEntryInstance.Get(entry.prefab_name);
            e = new RoadEntryInstance(entry);
            e.parts_type = e_type;
            e.cfg_entry = entry;
            if (prev_e != null)
            {
                if (entry.connect_direction.Contains("/"))
                {
                    string[] values = entry.connect_direction.Split('/');
                    prev_e = hash[values[1]];
                }

                CalculatePos(prev_e, e);
            }
            this.LoadPrefab(e);
            prev_e = e;
            hash[entry.prefab_name] = e;
        }
    }
    private void LoadPrefab(RoadEntryInstance road_entry)
    {
        var p = AssetDatabase.LoadAssetAtPath<GameObject>(road_entry.parts_type.prefab_path + "/" + road_entry.prefab_fname);
        road_entry.instance = Instantiate(p, road_entry.pos, Quaternion.identity) as GameObject;
        road_entry.instance.name = p.name + "_" + index;
        if (parent)
        {
            road_entry.instance.transform.parent = parent.transform;
        }
        Undo.RegisterCreatedObjectUndo(road_entry.instance, "Create Roads");

        road_entry.instance.transform.Rotate(0, road_entry.cfg_entry.rotation, 0);
        if (road_entry.cfg_entry.scale > 0.0f)
        {
            road_entry.instance.transform.localScale = new Vector3(road_entry.instance.transform.localScale.x, road_entry.instance.transform.localScale.y, road_entry.cfg_entry.scale);
        }
        var bounds = road_entry.instance.GetComponentInChildren<MeshRenderer>().bounds;
        Debug.Log(road_entry.prefab_fname + " : road_pos.instance scale=" + bounds.size);
        //Debug.Log("road_pos.rotation_angle=" + road_entry.cfg_entry.rotation);

        index++;
    }

}
