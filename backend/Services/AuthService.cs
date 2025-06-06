public async Task<AuthResponse> LoginUserAsync(LoginRequest request)
{
    return new AuthResponse
    {
        Success = true,
        Message = "Login bem-sucedido",
        IdToken = "fake-token",
        User = new UserInfo
        {
            Uid = "fake-uid",
            Email = request.Email,
            DisplayName = "Lucas",
            EmailVerified = true
        }
    };
}