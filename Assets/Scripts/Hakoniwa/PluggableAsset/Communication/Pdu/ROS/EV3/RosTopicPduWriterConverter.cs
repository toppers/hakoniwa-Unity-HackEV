using Hakoniwa.PluggableAsset.Communication.Pdu;
using Hakoniwa.PluggableAsset.Communication.Pdu.ROS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RosMessageTypes.Std;
using RosMessageTypes.Ev3;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using Hakoniwa.PluggableAsset.Communication.Pdu.ROS.EV3;
using RosMessageTypes.BuiltinInterfaces;

namespace Hakoniwa.PluggableAsset.Communication.Pdu.ROS.EV3
{
    class RosTopicPduWriterConverter : IPduWriterConverter
    {
        public IPduCommData ConvertToIoData(IPduWriter src)
        {
            RosTopicPduWriter pdu_writer = src as RosTopicPduWriter;
            return new RosTopicPduCommTypedData(pdu_writer);
        }
        

        static private void ConvertToMessage(IPduReadOperation src, Ev3PduActuatorMsg dst)
        {
            ConvertToMessage(src.Ref("header").GetPduReadOps(), dst.header);
			dst.leds = src.GetDataUInt8Array("leds");
            foreach (var e in src.Refs("motors"))
            {
                int index = Array.IndexOf(src.Refs("motors"), e);
                if (dst.motors[index] == null) {
                    dst.motors[index] = new Ev3PduMotorMsg();
                }
                ConvertToMessage(e.GetPduReadOps(), dst.motors[index]);
            }
			dst.gyro_reset = src.GetDataUInt32("gyro_reset");
        }
        static private void ConvertToMessage(IPduReadOperation src, Ev3PduActuatorHeaderMsg dst)
        {
			dst.name = src.GetDataString("name");
			dst.version = src.GetDataUInt32("version");
			dst.asset_time = src.GetDataInt64("asset_time");
			dst.ext_off = src.GetDataUInt32("ext_off");
			dst.ext_size = src.GetDataUInt32("ext_size");
        }
        static private void ConvertToMessage(IPduReadOperation src, Ev3PduColorSensorMsg dst)
        {
			dst.color = src.GetDataUInt32("color");
			dst.reflect = src.GetDataUInt32("reflect");
			dst.rgb_r = src.GetDataUInt32("rgb_r");
			dst.rgb_g = src.GetDataUInt32("rgb_g");
			dst.rgb_b = src.GetDataUInt32("rgb_b");
        }
        static private void ConvertToMessage(IPduReadOperation src, Ev3PduMotorMsg dst)
        {
			dst.power = src.GetDataInt32("power");
			dst.stop = src.GetDataUInt32("stop");
			dst.reset_angle = src.GetDataUInt32("reset_angle");
        }
        static private void ConvertToMessage(IPduReadOperation src, Ev3PduSensorMsg dst)
        {
            ConvertToMessage(src.Ref("header").GetPduReadOps(), dst.header);
			dst.buttons = src.GetDataUInt8Array("buttons");
            foreach (var e in src.Refs("color_sensors"))
            {
                int index = Array.IndexOf(src.Refs("color_sensors"), e);
                if (dst.color_sensors[index] == null) {
                    dst.color_sensors[index] = new Ev3PduColorSensorMsg();
                }
                ConvertToMessage(e.GetPduReadOps(), dst.color_sensors[index]);
            }
            foreach (var e in src.Refs("touch_sensors"))
            {
                int index = Array.IndexOf(src.Refs("touch_sensors"), e);
                if (dst.touch_sensors[index] == null) {
                    dst.touch_sensors[index] = new Ev3PduTouchSensorMsg();
                }
                ConvertToMessage(e.GetPduReadOps(), dst.touch_sensors[index]);
            }
			dst.motor_angle = src.GetDataUInt32Array("motor_angle");
			dst.gyro_degree = src.GetDataInt32("gyro_degree");
			dst.gyro_degree_rate = src.GetDataInt32("gyro_degree_rate");
			dst.sensor_ultrasonic = src.GetDataUInt32("sensor_ultrasonic");
			dst.gps_lat = src.GetDataFloat64("gps_lat");
			dst.gps_lon = src.GetDataFloat64("gps_lon");
        }
        static private void ConvertToMessage(IPduReadOperation src, Ev3PduSensorHeaderMsg dst)
        {
			dst.name = src.GetDataString("name");
			dst.version = src.GetDataUInt32("version");
			dst.hakoniwa_time = src.GetDataInt64("hakoniwa_time");
			dst.ext_off = src.GetDataUInt32("ext_off");
			dst.ext_size = src.GetDataUInt32("ext_size");
        }
        static private void ConvertToMessage(IPduReadOperation src, Ev3PduTouchSensorMsg dst)
        {
			dst.value = src.GetDataUInt32("value");
        }
        
        
        static public Message ConvertToMessage(IPduReadOperation src, string type)
        {

            if (type.Equals("Ev3PduSensor"))
            {
            	Ev3PduSensorMsg ros_topic = new Ev3PduSensorMsg();
                ConvertToMessage(src, ros_topic);
                return ros_topic;
            }
            if (type.Equals("Ev3PduActuator"))
            {
            	Ev3PduActuatorMsg ros_topic = new Ev3PduActuatorMsg();
                ConvertToMessage(src, ros_topic);
                return ros_topic;
            }
            throw new InvalidCastException("Can not find ros message type:" + type);
        }
        
        static public Message ConvertToMessage(RosTopicPduWriter pdu_writer)
        {
            return ConvertToMessage(pdu_writer.GetReadOps(), pdu_writer.GetTypeName());
        }
    }

}
