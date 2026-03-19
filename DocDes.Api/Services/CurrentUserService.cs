using DocDes.Core.Services;

namespace DocDes.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public int? UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value ??
                        _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

            return !string.IsNullOrEmpty(claim) ? Convert.ToInt32(claim) : null;
        }
    }
}