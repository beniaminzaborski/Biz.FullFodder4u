using Biz.FullFodder4u.Identity.API.DTOs;

namespace Biz.FullFodder4u.Identity.API.Services;

public interface IUserService
{
    Task SignUp(SignUpDataDto payload);

    Task<string> SignIn(SignInDataDto payload);
}
