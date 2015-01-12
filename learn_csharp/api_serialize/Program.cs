using System;
using System.Collections;
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
            //A ccc = new A();
            //ccc.a = 0;
            //if (ccc.GetType().IsClass)
            //{
            //    return;
            //}

            //int[] ccc = new int[0];
            //if (ccc.Length == 0)
            //{
            //    return;
            //}

            int[] ccc = new int[3];
            //object[] ccc1 = (object[])ccc;
            Array ccc1 = (Array)ccc;
            if (ccc1.Length == 0)
            {
                return;
            }


            //B b = SerializeTools.Deserialize<B>(SerializeTools.Serialize<B>(new B()));
            B bb = new B();
            bb.array_int = new int[3];
            bb.array_int[0] = 3;
            bb.array_a = new A[3];
            bb.array_a[1] = new A();
            bb.array_a[1].a = 1;
            bb.array_a[1].b = 1.1f;
            bb.array_a[1].c = true;
            bb.array_a[1].d = 3;
            bb.list_a = new List<List<A>>();
            bb.list_a.Add(new List<A>());
            bb.list_a[0].Add(new A());
            bb.list_a[0][0].b = 1.2f;
            bb.c.a = 2;
            bb.c.b = 4;
            //if (bb.c.a == 0)
            //{
            //    return;
            //}

            MLByteWriterBuffer buffer = new MLByteWriterBuffer();
            MLWriterStream writer = new MLByteWriterStream<MLByteWriterBuffer>(buffer);
            MLISerialize stream = new MLWriter(writer);
            SerializeTools.SerializeImpl(bb.GetType(), bb, stream);


            MLByteReaderBuffer buffer2 = new MLByteReaderBuffer(buffer.toByteArray());
            MLISerialize stream2 = new MLReader(new MLByteReaderStream<MLByteReaderBuffer>(buffer2));
            Object cc = null;
            SerializeTools.SerializeImpl(typeof(B), ref cc, stream2);
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
        public string ss;
        public int a;
        public int[] array_int;
        public A[] array_a;
        public List<List<A>> list_a;
        public C c;
        public Dictionary<A, int> dict_a;
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

        public static void SerializeImpl(Type type, object obj, MLISerialize stream)
        {
            SerializeImpl(type, ref obj, stream);
        }
        public static void SerializeImpl(Type type, ref object obj, MLISerialize stream)
        {
            if (type == typeof(string))
            {
                string iv = default(string);
                if (stream.IsReader)
                {
                    stream.Serialize(ref iv);
                    obj = iv;
                }
                else
                {
                    iv = (string)obj;
                    stream.Serialize(ref iv);
                }
            }
            else if (type == typeof(int))
            {
                int iv = default(int);
                if (stream.IsReader)
                {
                    stream.Serialize(ref iv);
                    obj = iv;
                }
                else
                {
                    iv = (int)obj;
                    stream.Serialize(ref iv);
                }
            }
            else if (type == typeof(short))
            {
                short iv = default(short);
                if (stream.IsReader)
                {
                    stream.Serialize(ref iv);
                    obj = iv;
                }
                else
                {
                    iv = (short)obj;
                    stream.Serialize(ref iv);
                }
            }
            else if (type == typeof(byte))
            {
                byte iv = default(byte);
                if (stream.IsReader)
                {
                    stream.Serialize(ref iv);
                    obj = iv;
                }
                else
                {
                    iv = (byte)obj;
                    stream.Serialize(ref iv);
                }
            }
            else if (type == typeof(double))
            {
                double iv = default(double);
                if (stream.IsReader)
                {
                    stream.Serialize(ref iv);
                    obj = iv;
                }
                else
                {
                    iv = (double)obj;
                    stream.Serialize(ref iv);
                }
            }
            else if (type == typeof(long))
            {
                long iv = default(long);
                if (stream.IsReader)
                {
                    stream.Serialize(ref iv);
                    obj = iv;
                }
                else
                {
                    iv = (long)obj;
                    stream.Serialize(ref iv);
                }
            }
            else if (type == typeof(float))
            {
                float iv = default(float);
                if (stream.IsReader)
                {
                    stream.Serialize(ref iv);
                    obj = iv;
                }
                else
                {
                    iv = (float)obj;
                    stream.Serialize(ref iv);
                }
            }
            else if (type == typeof(bool))
            {
                bool iv = default(bool);
                if (stream.IsReader)
                {
                    stream.Serialize(ref iv);
                    obj = iv;
                }
                else
                {
                    iv = (bool)obj;
                    stream.Serialize(ref iv);
                }
            }
            else if (type.IsArray)
            {
                // 数组格式，第一位为数量，0表示0个，-1表示null；第二位以后的表示具体元素
                // 数组存储的类型
                Type embedtype = type.GetElementType();

                int len = 0;
                if (stream.IsReader)
                {
                    stream.Serialize(ref len);
                    if (len == -1)  // -1表示空对象
                    {
                        obj = null;
                        return;
                    }
                    obj = Array.CreateInstance(embedtype, len);
                }
                else
                {
                    if (obj == null)
                    {
                        // 空对象第一个字段为-1
                        len = -1;
                        stream.Serialize(ref len);
                        return;
                    }
                    else
                    {
                        len = ((Array)obj).Length;
                        stream.Serialize(ref len);
                    }
                }

                // 序列化具体数据
                Array array_value = (Array)obj;
                for (int i = 0; i < len; i++)
                {
                    object o2 = null;
                    if (stream.IsReader)
                    {
                        SerializeImpl(embedtype, ref o2, stream);
                        array_value.SetValue(o2, i);
                    }
                    else
                    {
                        o2 = array_value.GetValue(i);
                        SerializeImpl(embedtype, ref o2, stream);
                    }
                }
            }
            else if (type.GetInterface("IList") != null)
            {
                // List和array存贮格式类似
                // List的格式，第一位为数量，0表示0个，-1表示null；第二位以后的表示具体元素
                // List存储的类型
                Type[] types = type.GetGenericArguments();
                Type embedtype = types[0];

                int len = 0;
                if (stream.IsReader)
                {
                    stream.Serialize(ref len);
                    if (len == -1)  // -1表示空对象
                    {
                        obj = null;
                        return;
                    }
                    obj = Activator.CreateInstance(type);
                }
                else
                {
                    if (obj == null)
                    {
                        // 空对象第一个字段为-1
                        len = -1;
                        stream.Serialize(ref len);
                        return;
                    }
                    else
                    {
                        len = ((IList)obj).Count;
                        stream.Serialize(ref len);
                    }
                }

                // 序列化具体数据
                IList array_value = (IList)obj;
                for (int i = 0; i < len; i++)
                {
                    object o2 = null;
                    if (!stream.IsReader)
                    {
                        o2 = array_value[i];
                    }
                    SerializeImpl(embedtype, ref o2, stream);
                    if (stream.IsReader)
                    {
                        array_value.Add(o2);
                    }
                }
            }
            else if (type.GetInterface("IDictionary") != null)
            {
                // 以后再写！
                Type[] types = type.GetGenericArguments();
                Console.WriteLine("xxxx filed:" + "IDictionary!");
            }
            else
            {
                // 自定义的类或者结构体！

                // 如果是自定义类，那么第一位要么为0，要么为1（0表示空对象，1表示初始化后的对象）
                if (type.IsClass)
                {
                    int len = 0;
                    if (stream.IsReader)
                    {
                        stream.Serialize(ref len);
                        if (len == 0)
                        {
                            obj = null;
                            return; // 空指针直接返回啦
                        }
                        else
                        {
                            obj = Activator.CreateInstance(type);
                        }
                    }
                    else
                    {
                        if (obj == null)
                        {
                            len = 0;
                            stream.Serialize(ref len);
                            return; // 空指针直接返回啦
                        }
                        else
                        {
                            len = 1;
                            stream.Serialize(ref len);
                        }
                    }
                }
                else
                {
                    // 结构体类型。先创建内存块咯
                    if (stream.IsReader)
                    {
                        obj = Activator.CreateInstance(type);
                    }
                }

                // 读取该类的所有数据！
                FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    // 如果是只读属性，那么过滤掉！


                    // Attribute.GetCustomAttribute
                    if (stream.IsReader)
                    {
                        Object obj_value = null;
                        SerializeImpl(field.FieldType, ref obj_value, stream);
                        field.SetValue(obj, obj_value);
                    }
                    else
                    {
                        SerializeImpl(field.FieldType, field.GetValue(obj), stream);
                    }
                }
            }
        }


    }
}
