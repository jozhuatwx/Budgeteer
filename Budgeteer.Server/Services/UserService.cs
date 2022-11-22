using AutoMapper;

namespace Budgeteer.Server.Services;

public class UserService : IUserService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public UserService(
        UnitOfWork unitOfWork,
        INotificationService notificationService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public async Task<ICollection<UserResponse>> GetUsersAsync(CancellationToken cancellationToken = default) =>
        await _unitOfWork.Users.GetAllAsync<UserResponse>(_mapper.ConfigurationProvider, cancellationToken: cancellationToken);

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = new User()
        {
            Name = request.Name,
            Email = request.Email.ToLower(),
            HashedPassword = await CryptographyUtility.HashPasswordAsync(request.Password, cancellationToken)
        };

        _unitOfWork.Users.Create(user);
        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse?> GetUserAsync(int id, CancellationToken cancellationToken = default) =>
        await _unitOfWork.Users.GetAsync<UserResponse>(id, _mapper.ConfigurationProvider, cancellationToken: cancellationToken);

    public async Task<UserResponse?> UpdateUserAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetAsync<User>(id, _mapper.ConfigurationProvider, track: true, cancellationToken: cancellationToken);

        if (user == null)
        {
            return null;
        }

        user.Name = request.Name;
        user.Email = request.Email.ToLower();

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveAsync(cancellationToken);

        await _notificationService.SendIndividualAsync(id, "Updated user!", cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse?> DeleteUserAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetAsync<User>(id, _mapper.ConfigurationProvider, track: true, cancellationToken: cancellationToken);

        if (user == null)
        {
            return null;
        }

        _unitOfWork.Users.Delete(user);
        await _unitOfWork.SaveAsync(cancellationToken);

        await _notificationService.SendIndividualAsync(id, "Deleted user!", cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }
}
