using System;
using AmberMeet.Dto.Meets;

namespace AmberMeet.AppService.Meets
{
    public interface IMeetCommandService
    {
        string AddMeet(MeetDto dto);

        void ChangeMeet(MeetDto dto);

        void ActivateMeet(string meetId, DateTime startTime, DateTime? endTime, string place);
    }
}