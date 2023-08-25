using System.Runtime.Serialization;

namespace RentCarSys.Domain.Enums
{
    public enum ReservaStatus
    {
        [EnumMember(Value = "Online")]
        Online = 1,
        [EnumMember(Value = "Running")]
        Running = 2,
        [EnumMember(Value = "Offline")]
        Offline = 3,
    }
}
