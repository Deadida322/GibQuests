using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestService.ProcessQuestDatabase.Interfaces;
using ProcessQuestService.ProcessQuestDatabase.Models;

namespace ProcessQuestService.ProcessQuestDatabase.Implements
{
    public class ProcessQuestStorage : IProcessQuestStorage
    {
        private ProcessQuestContext _context;
        private IMapper _mapper;
        public ProcessQuestStorage(ProcessQuestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateRoomAsync(RegisterRoomContract contract)
        {
            var room = _mapper.Map<RoomEntity>(contract);
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public Task<bool> IsRoomExist(Guid roomKey)
        {
            return _context.Rooms.Where(r => r.Key == roomKey).AnyAsync();
        }
    }
}
