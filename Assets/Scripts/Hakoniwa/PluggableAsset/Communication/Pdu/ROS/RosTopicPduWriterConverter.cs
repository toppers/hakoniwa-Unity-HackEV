using Hakoniwa.PluggableAsset.Communication.Pdu;
using Hakoniwa.PluggableAsset.Communication.Pdu.ROS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RosMessageTypes.Std;
using RosMessageTypes.Hackev;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace Hakoniwa.PluggableAsset.Communication.Pdu.ROS
{
    class RosTopicPduWriterConverter : IPduWriterConverter
    {
        public IPduCommData ConvertToIoData(IPduWriter src)
        {
            RosTopicPduWriter pdu_writer = src as RosTopicPduWriter;
            return new RosTopicPduCommTypedData(pdu_writer);
        }
        

        static private void ConvertToMessage(IPduReadOperation src, MActuator dst)
        {
			dst.led = src.GetDataInt32("led");
			dst.motor_power_a = src.GetDataInt32("motor_power_a");
			dst.motor_power_b = src.GetDataInt32("motor_power_b");
			dst.motor_power_c = src.GetDataInt32("motor_power_c");
			dst.motor_stop_a = src.GetDataUInt32("motor_stop_a");
			dst.motor_stop_b = src.GetDataUInt32("motor_stop_b");
			dst.motor_stop_c = src.GetDataUInt32("motor_stop_c");
			dst.motor_reset_angle_a = src.GetDataInt32("motor_reset_angle_a");
			dst.motor_reset_angle_b = src.GetDataInt32("motor_reset_angle_b");
			dst.motor_reset_angle_c = src.GetDataInt32("motor_reset_angle_c");
			dst.gyro_reset = src.GetDataInt32("gyro_reset");
        }
        static private void ConvertToMessage(IPduReadOperation src, MHeader dst)
        {
			dst.seq = src.GetDataUInt32("seq");
            ConvertToMessage(src.Ref("stamp").GetPduReadOps(), dst.stamp);
			dst.frame_id = src.GetDataString("frame_id");
        }
        static private void ConvertToMessage(IPduReadOperation src, MLaserScan dst)
        {
            ConvertToMessage(src.Ref("header").GetPduReadOps(), dst.header);
			dst.angle_min = src.GetDataFloat32("angle_min");
			dst.angle_max = src.GetDataFloat32("angle_max");
			dst.angle_increment = src.GetDataFloat32("angle_increment");
			dst.time_increment = src.GetDataFloat32("time_increment");
			dst.scan_time = src.GetDataFloat32("scan_time");
			dst.range_min = src.GetDataFloat32("range_min");
			dst.range_max = src.GetDataFloat32("range_max");
			dst.ranges = src.GetDataFloat32Array("ranges");
			dst.intensities = src.GetDataFloat32Array("intensities");
        }
        static private void ConvertToMessage(IPduReadOperation src, MTime dst)
        {
			dst.secs = src.GetDataUInt32("secs");
			dst.nsecs = src.GetDataUInt32("nsecs");
        }
        
        
        static public Message ConvertToMessage(IPduReadOperation src, string type)
        {

            if (type.Equals("LaserScan"))
            {
            	MLaserScan ros_topic = new MLaserScan();
                ConvertToMessage(src, ros_topic);
                return ros_topic;
            }
            if (type.Equals("Actuator"))
            {
            	MActuator ros_topic = new MActuator();
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
