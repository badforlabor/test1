using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace api_serialize
{
    class Program
    {
        static void Main(string[] args)
        {
            //B b = SerializeTools.Deserialize<B>(SerializeTools.Serialize<B>(new B()));
            B bb = new B();
            bb.array_a = new A[3];
            //bb.array_a[0].a = 1;

            MLByteWriterBuffer buffer = new MLByteWriterBuffer();
            MLWriterStream writer = new MLByteWriterStream<MLByteWriterBuffer>(buffer);
            MLISerialize stream = new MLWriter(writer);
            SerializeTools.SerializeImpl(bb, stream);


            MLByteReaderBuffer buffer2 = new MLByteReaderBuffer(buffer.toByteArray());
            MLISerialize stream2 = new MLReader(new MLByteReaderStream<MLByteReaderBuffer>(buffer2));
            B cc = new B();
            SerializeTools.SerializeImpl(cc, stream2);
        }
    }
    struct C
    {
        public int a;
        public long b;
    }
    class A
    {
        public int a;
        public float b;
        public bool c;
        public long d;
    }
    class B
    {
        public int a;
        public A[] array_a;
        public List<A> list_a;
        public C c;
    }
    class SerializeTools
    {
        static void ShowSubFields(Type tp)
        {
            FieldInfo[] fields = tp.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                Console.WriteLine("     sub filed:" + field.FieldType + ", ");
            }
        }
        public static string Serialize<T>(T obj)
        {
            StringBuilder buffer = new StringBuilder();

            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(int))
                {
                    Console.WriteLine("xxxx filed:" + "int!");
                }
                else if(field.FieldType.IsArray)
                {
                    Type types = field.FieldType.GetElementType();
                    Console.WriteLine("xxxx filed:" + "array!" + types);
                }
                else if (field.FieldType.GetInterface("IList") != null)
                {
                    Type[] types = field.FieldType.GetGenericArguments();
                    Console.WriteLine("xxxx filed:" + "list!");
                }
                ShowSubFields(field.FieldType);
                Console.WriteLine("xxxx filed:" + field.FieldType + ", ");
            }

            return "";
        }
        public static T Deserialize<T>(string buffer)
        {
            return default(T);
        }

        public static void SerializeImpl(object obj, MLISerialize stream)
        {
            if (obj == null)
            {
                //throw new Exception("critical!.");
                return;
            }
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                // Attribute.GetCustomAttribute
                if (field.FieldType.IsArray)
                {
                    // 写入：数组长度（如果是null，那么写-1）
                    Type embedtype = field.FieldType.GetElementType();
                    if (stream.IsReader)
                    {
                        int len = 0;
                        stream.Serialize(ref len);
                        object val = field.GetValue(obj);
                        if (len == -1)
                        {
                            field.SetValue(obj, null);
                        }
                        else
                        {
                            object[] array_value = (object[]) Array.CreateInstance(embedtype, len);

                            for(int i=0; i<len; i++)
                            {
                                //if (array_value[i] == null)
                                //{
                                //    throw new Exception("critical! new failed.");
                                //}
                                object o2 = array_value[i];
                                SerializeImpl(o2, stream);
                            }
                        }
                    }
                    else
                    {
                        object val = field.GetValue(obj);
                        if (val == null)
                        {
                            int len = -1;
                            stream.Serialize(ref len);
                        }
                        else
                        {
                            object[] array_value = (object[])val;
                            int len = array_value.Length;
                            stream.Serialize(ref len);
                            foreach (var o2 in array_value)
                            {
                                SerializeImpl(o2, stream);
                            }
                        }
                    }
                }
            }
        }


    }
}
