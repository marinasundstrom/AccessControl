using System;
namespace AppService.Application.Rfid.Commands
{
    public class TagDataDto
    {
        public TagDataDto(byte[] uid)
        {
            UID = uid;
        }

        public byte[] UID { get; }
    }
}
