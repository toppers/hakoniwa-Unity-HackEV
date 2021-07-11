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
using Hakoniwa.PluggableAsset.Communication.Pdu.ROS.EV3_TB3;

namespace Hakoniwa.PluggableAsset.Communication.Pdu.ROS.EV3_TB3
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
        private void ConvertToPdu(MHeader src, IPduWriteOperation dst)
        {
            dst.SetData("seq", src.seq);
			ConvertToPdu(src.stamp, dst.Ref("stamp").GetPduWriteOps());
            dst.SetData("frame_id", src.frame_id);
        }
        private void ConvertToPdu(MImu src, IPduWriteOperation dst)
        {
			ConvertToPdu(src.header, dst.Ref("header").GetPduWriteOps());
			ConvertToPdu(src.orientation, dst.Ref("orientation").GetPduWriteOps());
            dst.SetData("orientation_covariance", src.orientation_covariance);
			ConvertToPdu(src.angular_velocity, dst.Ref("angular_velocity").GetPduWriteOps());
            dst.SetData("angular_velocity_covariance", src.angular_velocity_covariance);
			ConvertToPdu(src.linear_acceleration, dst.Ref("linear_acceleration").GetPduWriteOps());
            dst.SetData("linear_acceleration_covariance", src.linear_acceleration_covariance);
        }
        private void ConvertToPdu(MLaserScan src, IPduWriteOperation dst)
        {
			ConvertToPdu(src.header, dst.Ref("header").GetPduWriteOps());
            dst.SetData("angle_min", src.angle_min);
            dst.SetData("angle_max", src.angle_max);
            dst.SetData("angle_increment", src.angle_increment);
            dst.SetData("time_increment", src.time_increment);
            dst.SetData("scan_time", src.scan_time);
            dst.SetData("range_min", src.range_min);
            dst.SetData("range_max", src.range_max);
            dst.SetData("ranges", src.ranges);
            dst.SetData("intensities", src.intensities);
        }
        private void ConvertToPdu(MQuaternion src, IPduWriteOperation dst)
        {
            dst.SetData("x", src.x);
            dst.SetData("y", src.y);
            dst.SetData("z", src.z);
            dst.SetData("w", src.w);
        }
        private void ConvertToPdu(MTime src, IPduWriteOperation dst)
        {
            dst.SetData("secs", src.secs);
            dst.SetData("nsecs", src.nsecs);
        }
        private void ConvertToPdu(MTwist src, IPduWriteOperation dst)
        {
			ConvertToPdu(src.linear, dst.Ref("linear").GetPduWriteOps());
			ConvertToPdu(src.angular, dst.Ref("angular").GetPduWriteOps());
        }
        private void ConvertToPdu(MVector3 src, IPduWriteOperation dst)
        {
            dst.SetData("x", src.x);
            dst.SetData("y", src.y);
            dst.SetData("z", src.z);
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
            if (ros_pdu_reader.GetTypeName().Equals("LaserScan"))
            {
                var ros_topic_data = ros_topic.GetTopicData() as MLaserScan;
                ConvertToPdu(ros_topic_data, dst.GetWriteOps());
                return;
            }
            if (ros_pdu_reader.GetTypeName().Equals("Imu"))
            {
                var ros_topic_data = ros_topic.GetTopicData() as MImu;
                ConvertToPdu(ros_topic_data, dst.GetWriteOps());
                return;
            }
            if (ros_pdu_reader.GetTypeName().Equals("Twist"))
            {
                var ros_topic_data = ros_topic.GetTopicData() as MTwist;
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
