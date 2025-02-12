public enum NikonOperationCodes
{
    OpenSession = 0x1002,
    GetDeviceInfo = 0x1001,
    SyncDate = 0x1016,
    SessionAlreadyOpen = 0x201e,
    DevicePropNotSupported = 0x200a,
    CloseSession = 0x1003,
    GetStorageIds = 0x1004,
    RequestListOfPictures = 0x1007,
    RequestThumb = 0x90c4,//Migght be request thumb 
    RequestPhoto = 0x0001,
    RequestFullPicture = 0x101b,
    EndDataTransfer = 0x90c7//Not sure if that's what this is
}
public enum NikonResponseCodes
{
    DownloadData = 0,
    DownloadPhoto = 1,
    //    static Int16[] opCodes = new short[] { 0x0110, 0x0210, 0x1016 };
    ListOfPictureID = 0x0364,
    InvalidObjectHandle = 8201,
    DataBegin = 0x4a5a
}

public enum TCPPacketType
{
    RequestOperation = 6
}