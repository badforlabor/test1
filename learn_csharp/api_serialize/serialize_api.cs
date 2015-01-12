using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;

namespace api_serialize
{
    public interface MLISerialize
    {
        // 基础数据类型
        void Serialize(ref int a);
        void Serialize(ref uint a);
        void Serialize(ref long a);
        void Serialize(ref string a);
        void Serialize(ref bool a);
        void Serialize(ref float a);
        void Serialize(ref byte a);
        void Serialize(ref short a);
        void Serialize(ref double a);

        void Serialize(ref string[] a);

        bool IsReader
        {
            get;
        }
    }
    public interface MLWriterStream
    {
        void writeByte(byte data);

        void writeShort(short data);

        void writeShort(int value);

        void writeInteger(int data);

        void writeLong(long data);

        void writeFloat(float data);

        void writeDouble(double data);

        void writeBoolean(bool data);

        void writeString(String str);

        void writeString(String str, String charset);

        void writeBytes(byte[] datas);

        void writeByteArray(byte[] datas);

        byte[] get();

        int Position
        {
            set;
            get;
        }
    }

    public interface MLReaderStream
    {
        byte readByte();

        short readShort();

        int readInteger();

        long readLong();

        float readFloat();

        double readDouble();

        bool readBoolean();

        String readString();

        String readString(String charset);

        byte[] readBytes();

        byte[] readByteArray();

        int Position
        {
            set;
            get;
        }
        int Remaining
        {
            get;
        }
    }
    public class MLReader : MLISerialize
    {
        MLReaderStream mStream = null;

        public MLReader(MLReaderStream stream)
        {
            mStream = stream;
        }
        public bool IsReader
        {
            get { return true; }
        }

        public void Serialize(ref int a)
        {
            a = mStream.readInteger();
        }
        public void Serialize(ref uint a)
        {
            a = (uint)mStream.readInteger();
        }
        public void Serialize(ref long a)
        {
            a = mStream.readLong();
        }
        public void Serialize(ref byte a)
        {
            a = mStream.readByte();
        }
        public void Serialize(ref short a)
        {
            a = mStream.readShort();
        }
        public void Serialize(ref double a)
        {
            a = mStream.readDouble();
        }
        public void Serialize(ref string a)
        {
            a = mStream.readString();
        }
        public void Serialize(ref bool a)
        {
            a = mStream.readBoolean();
        }
        public void Serialize(ref float a)
        {
            a = mStream.readFloat();
        }
        public void Serialize(ref string[] a)
        {
            int length = mStream.readInteger();
            a = new string[length];
            for (int i = 0; i < length; i++)
            {
                a[i] = mStream.readString();
            }
        }
    }

    public interface MLReaderBuffer
    {
        int Position { set; get; }
        int Remaining { get; }
        byte readByte();
        short readShort();
        int readInt();
        long readLong();
        float readFloat();
        double readDouble();
        bool readBool();
        string readString(string charset);
        byte[] readBytes();
        byte[] readByteArray();
    }
    public interface MLWriterBuffer
    {
        int Position { set; get; }
        byte[] toByteArray();
        void writeByte(byte by);
        void writeBytes(byte[] byteArray);
        void writeByteArray(byte[] byteArray);
        void writeShort(short num);
        void writeInt(int num);
        void writeLong(long num);
        void writeFloat(float num);
        void writeDouble(double num);
        void writeBoolean(bool num);
        void writeString(string str, string charset);
    }
    public class MLWriter : MLISerialize
    {
        MLWriterStream mStream = null;

        public MLWriter(MLWriterStream stream)
        {
            mStream = stream;
        }

        public bool IsReader
        {
            get { return false; }
        }

        public void Serialize(ref int a)
        {
            mStream.writeInteger(a);
        }
        public void Serialize(ref uint a)
        {
            mStream.writeInteger((int)a);
        }
        public void Serialize(ref long a)
        {
            mStream.writeLong(a);
        }
        public void Serialize(ref byte a)
        {
            mStream.writeByte(a);
        }
        public void Serialize(ref short a)
        {
            mStream.writeShort(a);
        }
        public void Serialize(ref double a)
        {
            mStream.writeDouble(a);
        }
        public void Serialize(ref string a)
        {
            mStream.writeString(a);
        }
        public void Serialize(ref bool a)
        {
            mStream.writeBoolean(a);
        }
        public void Serialize(ref float a)
        {
            mStream.writeFloat(a);
        }
        public void Serialize(ref string[] a)
        {
            mStream.writeInteger(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                mStream.writeString(a[i]);
            }
        }
    }
    public class MLByteWriterStream<T> : MLWriterStream where T : class,MLWriterBuffer
    {
        private T buf = null;
        public MLByteWriterStream(T buf)
        {
            this.buf = buf;

        }

        public void writeByte(byte data)
        {
            buf.writeByte(data);
        }

        public void writeShort(short data)
        {
            buf.writeShort(data);
        }

        public void writeShort(int value)
        {
            buf.writeShort((short)value);
        }

        public void writeInteger(int data)
        {
            buf.writeInt(data);
        }

        public void writeLong(long data)
        {
            buf.writeLong(data);
        }

        public void writeFloat(float data)
        {
            buf.writeFloat(data);
        }

        public void writeDouble(double data)
        {
            buf.writeDouble(data);
        }

        public void writeBoolean(bool data)
        {
            buf.writeBoolean(data);
        }

        public void writeString(String str)
        {
            buf.writeString(str, "utf-8");
        }

        public void writeString(String str, String charset)
        {
            buf.writeString(str, charset);
        }

        public void writeBytes(byte[] datas)
        {
            buf.writeBytes(datas);
        }

        public void writeByteArray(byte[] datas)
        {
            buf.writeByteArray(datas);
        }

        public byte[] get()
        {
            return buf.toByteArray();
        }

        public int Position
        {
            set
            {
                buf.Position = value;
            }
            get
            {
                return buf.Position;
            }
        }
    }
    public class MLByteReaderStream<T> : MLReaderStream where T : class,MLReaderBuffer
    {
        private T buf = null;

        public MLByteReaderStream(T buf)
        {
            this.buf = buf;
            buf.Position = 0;
        }

        public byte readByte()
        {
            return buf.readByte();
        }

        public short readShort()
        {
            return buf.readShort();
        }

        public int readInteger()
        {
            return buf.readInt();
        }

        public long readLong()
        {
            return buf.readLong();
        }

        public float readFloat()
        {
            return buf.readFloat();
        }

        public double readDouble()
        {
            return buf.readDouble();
        }

        public bool readBoolean()
        {
            return buf.readBool();
        }

        public String readString()
        {
            return buf.readString("utf-8");
        }

        public String readString(String charset)
        {
            return buf.readString(charset);
        }

        public byte[] readBytes()
        {
            return buf.readBytes();
        }


        public byte[] readByteArray()
        {
            return buf.readByteArray();
        }

        public int Position
        {
            set
            {
                buf.Position = value;
            }
            get
            {
                return buf.Position;
            }
        }
        public int Remaining
        {
            get
            {
                return buf.Remaining;
            }
        }
    }


    public class MLByteReaderBuffer : MLReaderBuffer
    {
        private byte[] _data;

        public int Position { set; get; }

        // !< 一些常量
        protected static byte ShortSize = sizeof(short);
        protected static byte IntSize = sizeof(int);
        protected static byte LongSize = sizeof(long);
        protected static byte FloatSize = sizeof(float);
        protected static byte DoubleSize = sizeof(double);
        protected static byte DecimalSize = sizeof(decimal);

        public int Remaining
        {
            get
            {
                return _data.Length - Position;
            }
        }

        public MLByteReaderBuffer(byte[] bytes)
        {
            _data = bytes;
            Position = 0;
        }

        public byte readByte()
        {
            return _data[Position++];
        }
        public short readShort()
        {
            short ret = BitConverter.ToInt16(_data, Position);
            Position += ShortSize;
            return ret;
        }
        public int readInt()
        {
            int ret = BitConverter.ToInt32(_data, Position);
            Position += IntSize;
            return ret;
        }
        public long readLong()
        {
            long ret = BitConverter.ToInt64(_data, Position);
            Position += LongSize;
            return ret;
        }
        public float readFloat()
        {
            float ret = BitConverter.ToSingle(_data, Position);
            Position += FloatSize;
            return ret;
        }
        public double readDouble()
        {
            double ret = BitConverter.ToDouble(_data, Position);
            Position += DoubleSize;
            return ret;
        }
        public bool readBool()
        {
            return BitConverter.ToBoolean(_data, Position++);
        }
        public string readString(string charset)
        {
            short len = readShort();
            string ret = System.Text.Encoding.GetEncoding(charset).GetString(_data, Position, len);
            Position += len;
            return ret;
        }
        public byte[] readBytes()
        {
            return _data;
        }
        public byte[] readByteArray()
        {
            int len = readInt();
            byte[] ret = new byte[len];
            Array.Copy(_data, Position, ret, 0, len);
            Position += len;
            return ret;
        }
    }

    public class MLByteWriterBuffer : MLWriterBuffer
    {
        private ArrayList tempByteArray = new ArrayList();

        public int Position { set; get; }

        public void clear()
        {
            tempByteArray.Clear();
        }

        public byte[] toByteArray()
        {
            return (byte[])tempByteArray.ToArray(typeof(byte));
        }

        public void writeByte(byte by)
        {
            tempByteArray.Insert(Position++, by);
        }

        public void writeBytes(byte[] byteArray)
        {
            tempByteArray.InsertRange(Position, byteArray);
            Position += byteArray.Length;
        }
        public void writeByteArray(byte[] byteArray)
        {
            if (byteArray != null && byteArray.Length > 0)
            {
                int length = byteArray.Length;
                writeInt(length);
                writeBytes(byteArray);
            }
            else
            {
                writeInt(0);
            }
        }
        public void writeShort(short num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            tempByteArray.InsertRange(Position, bytes);
            Position += bytes.Length;
        }
        public void writeInt(int num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            tempByteArray.InsertRange(Position, bytes);
            Position += bytes.Length;
        }
        public void writeLong(long num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            tempByteArray.InsertRange(Position, bytes);
            Position += bytes.Length;
        }
        public void writeFloat(float num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            tempByteArray.InsertRange(Position, bytes);
            Position += bytes.Length;
        }
        public void writeDouble(double num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            tempByteArray.InsertRange(Position, bytes);
            Position += bytes.Length;
        }
        public void writeBoolean(bool num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            tempByteArray.InsertRange(Position, bytes);
            Position += bytes.Length;
        }
        public void writeString(string str, string charset)
        {
            if (str == null)
            {
                writeShort((short)0);
                return;
            }
            byte[] bytes = System.Text.Encoding.GetEncoding(charset).GetBytes(str);
            writeShort((short)bytes.Length);
            writeBytes(bytes);
        }
    }

}
