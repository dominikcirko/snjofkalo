using AutoMapper;
using OICAR.DAL.Repository.Interface;
using OICAR.DTOs;
using OICAR.Models;
using OICAR.Service.Interface;


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDTO>>(users);
    }

    public async Task<UserDTO> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserDTO>(user);
    }

    public async Task AddAsync(UserDTO userDto)
    {
        var user = _mapper.Map<User>(userDto);
        await _userRepository.AddAsync(user);
    }

    public async Task UpdateAsync(UserDTO userDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(userDto.IDUser);
        if (existingUser == null)
        {
            throw new Exception($"User with ID {userDto.IDUser} not found.");
        }

        existingUser.Username = userDto.Username;
        existingUser.FirstName = userDto.FirstName;
        existingUser.LastName = userDto.LastName;
        existingUser.Email = userDto.Email;
        existingUser.PhoneNumber = userDto.PhoneNumber;

        await _userRepository.UpdateAsync(existingUser);
    }



    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        await _userRepository.DeleteAsync(id);
    }

    public async Task<UserDTO> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<IEnumerable<UserDTO>> GetAdminsAsync()
    {
        var admins = await _userRepository.GetAdminsAsync();
        return _mapper.Map<IEnumerable<UserDTO>>(admins);
    }
}