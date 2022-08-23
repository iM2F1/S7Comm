# S7Communication Test

- using
    - using S7.Net;
- Initial a instance
    - public Plc(CpuType cpu, string ip, port, Int16 rack, Int16 slot)
        - var plc = new Plc(CpuType.S71500, "127.0.0.1", 105, 0, 1);
    - open()
        - plc.open();
- Read
    - public byte[] ReadBytes(DataType dataType, int db, int startByteAdr, int count)
        - plc.ReadBytes(DataType.DataBlock, DBid, 0, testSize);
- Write
    - public void WriteBytes(DataType dataType, int db, int startByteAdr, byte[] value)
        - plc.WriteBytes(DataType.DataBlock, DBid, 0, dummylist.ToArray());
