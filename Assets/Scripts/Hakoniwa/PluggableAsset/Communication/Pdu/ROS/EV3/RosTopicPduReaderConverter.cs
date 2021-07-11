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

namespace Hakoniwa.PluggableAsset.Communication.Pdu.ROS.EV3
{
    class RosTopicPduReaderConverter : IPduReaderConverter
    {
        public IPduCommData ConvertToIoData(IPduReader src)
        {
            RosTopicPduReader pdu_reader = src as RosTopicPduReader;
            return new RosTopicPduCommTypedData(pdu_reader);
        }
        

        private void ConvertToPdu(MEv3PduActuator src, IPduWriteOperation dst)
        {
			ConvertToPdu(src.header, dst.Ref("header").GetPduWriteOps());
            dst.SetData("leds", src.leds);
            foreach (var e in dst.Refs("motors"))
            {
                int index = Array.IndexOf(dst.Refs("motors"), e);
                if (src.motors[index] == null) {
                    src.motors[index] = new MEv3PduMotor();
                }
				ConvertToPdu(src.motors[index], e.GetPduWriteOps());
            }
            dst.SetData("gyro_reset", src.gyro_reset);
        }
        private void ConvertToPdu(MEv3PduActuatorHeader src, IPduWriteOperation dst)
        {
            dst.SetData("name", src.name);
            dst.SetData("version", src.version);
            dst.SetData("asset_time", src.asset_time);
            dst.SetData("ext_off", src.ext_off);
            dst.SetData("ext_size", src.ext_size);
        }
        private void ConvertToPdu(MEv3PduColorSensor src, IPduWriteOperation dst)
        {
            dst.SetData("color", src.color);
            dst.SetData("reflect", src.reflect);
            dst.SetData("rgb_r", src.rgb_r);
            dst.SetData("rgb_g", src.rgb_g);
            dst.SetData("rgb_b", src.rgb_b);
        }
        private void ConvertToPdu(MEv3PduMotor src, IPduWriteOperation dst)
        {
            dst.SetData("power", src.power);
            dst.SetData("stop", src.stop);
            dst.SetData("reset_angle", src.reset_angle);
        }
        private void ConvertToPdu(MEv3PduSensor src, IPduWriteOperation dst)
        {
			ConvertToPdu(src.header, dst.Ref("header").GetPduWriteOps());
            dst.SetData("buttons", src.buttons);
            foreach (var e in dst.Refs("color_sensors"))
            {
                int index = Array.IndexOf(dst.Refs("color_sensors"), e);
                if (src.color_sensors[index] == null) {
                    src.color_sensors[index] = new MEv3PduColorSensor();
                }
				ConvertToPdu(src.color_sensors[index], e.GetPduWriteOps());
            }
            foreach (var e in dst.Refs("touch_sensors"))
            {
                int index = Array.IndexOf(dst.Refs("touch_sensors"), e);
                if (src.touch_sensors[index] == null) {
                    src.touch_sensors[index] = new MEv3PduTouchSensor();
                }
				ConvertToPdu(src.touch_sensors[index], e.GetPduWriteOps());
            }
            dst.SetData("motor_angle", src.motor_angle);
            dst.SetData("gyro_degree", src.gyro_degree);
            dst.SetData("gyro_degree_rate", src.gyro_degree_rate);
            dst.SetData("sensor_ultrasonic", src.sensor_ultrasonic);
            dst.SetData("gps_lat", src.gps_lat);
            dst.SetData("gps_lon", src.gps_lon);
        }
        private void ConvertToPdu(MEv3PduSensorHeader src, IPduWriteOperation dst)
        {
            dst.SetData("name", src.name);
            dst.SetData("version", src.version);
            dst.SetData("hakoniwa_time", src.hakoniwa_time);
            dst.SetData("ext_off", src.ext_off);
            dst.SetData("ext_size", src.ext_size);
        }
        private void ConvertToPdu(MEv3PduTouchSensor src, IPduWriteOperation dst)
        {
            dst.SetData("value", src.value);
        }

        public void ConvertToPduData(IPduCommData src, IPduReader dst)
        {
            RosTopicPduCommTypedData ros_topic = src as RosTopicPduCommTypedData;

            RosTopicPduReader ros_pdu_reader = dst as RosTopicPduReader;

            if (ros_pdu_reader.GetTypeName().Equals("Ev3PduSensor"))
            {
                var ros_topic_data = ros_topic.GetTopicData() as MEv3PduSensor;
                ConvertToPdu(ros_topic_data, dst.GetWriteOps());
                return;
            }
            if (ros_pdu_reader.GetTypeName().Equals("Ev3PduActuator"))
            {
                var ros_topic_data = ros_topic.GetTopicData() as MEv3PduActuator;
                ConvertToPdu(ros_topic_data, dst.GetWriteOps());
                return;
            }
            throw new InvalidCastException("Can not find ros message type:" + ros_pdu_reader.GetTypeName());

        }
        static public Message ConvertToMessage(RosTopicPduReader pdu_reader)
        {
            return RosTopicPduWriterConverter.ConvertToMessage(pdu_reader.GetReadOps(), pdu_reader.GetTypeName());
        }
    }
}
