using AutoMapper;
using OICAR.DAL.Repository.Interface;
using OICAR.DTOs;
using OICAR.Models;
using OICAR.Service.Interface;

namespace OICAR.Service.Implementation
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, ILogService logService, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDTO>> GetAllAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            _logService.LogAction("Info", "Retrieved all tags.");
            return _mapper.Map<IEnumerable<TagDTO>>(tags);
        }

        public async Task<TagDTO> GetByIdAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);

            if (tag == null)
            {
                _logService.LogAction("Warning", $"Tag with id={id} not found.");
                return null;
            }

            _logService.LogAction("Info", $"Retrieved tag with id={id}.");
            return _mapper.Map<TagDTO>(tag);
        }

        public async Task AddAsync(TagDTO tagDto)
        {
            var tag = _mapper.Map<Tag>(tagDto);
            await _tagRepository.AddAsync(tag);
            _logService.LogAction("Info", $"Tag with id={tag.Idtag} has been added.");
        }

        public async Task UpdateAsync(TagDTO tagDto)
        {
            var tag = _mapper.Map<Tag>(tagDto);
            await _tagRepository.UpdateAsync(tag);
            _logService.LogAction("Info", $"Tag with id={tag.Idtag} has been updated.");
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);

            if (tag == null)
            {
                _logService.LogAction("Warning", $"Attempted to delete non-existent tag with id={id}.");
                return;
            }

            await _tagRepository.DeleteAsync(id);
            _logService.LogAction("Info", $"Tag with id={id} has been deleted.");
        }

        public async Task<TagDTO> GetByNameAsync(string name)
        {
            var tag = await _tagRepository.GetByNameAsync(name);

            if (tag == null)
            {
                _logService.LogAction("Warning", $"Tag with name '{name}' not found.");
                return null;
            }

            _logService.LogAction("Info", $"Retrieved tag with name '{name}'.");
            return _mapper.Map<TagDTO>(tag);
        }
    }
}