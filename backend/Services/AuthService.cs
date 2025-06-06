namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly FirebaseAuth _firebaseAuth;
        private readonly ILogger<AuthService> _logger;
        private readonly string _firebaseApiKey;
        private readonly HttpClient _httpClient = new();

        // Construtor...

        // Métodos...

        public async Task<AuthResponse> LoginUserAsync(LoginRequest request)
        {
            try
            {
                var payload = new
                {
                    email = request.Email,
                    password = request.Password,
                    returnSecureToken = true
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_firebaseApiKey}",
                    payload);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorObj = JsonDocument.Parse(errorContent);
                    var errorCode = errorObj.RootElement
                                            .GetProperty("error")
                                            .GetProperty("message")
                                            .GetString();

                    _logger.LogWarning("Falha no login: {ErrorCode}", errorCode);
                    return new AuthResponse
                    {
                        Success = false,
                        Message = GetFirebaseErrorMessage(errorCode)
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                var idToken = result.GetProperty("idToken").GetString();
                var uid = result.GetProperty("localId").GetString();

                var userRecord = await _firebaseAuth.GetUserAsync(uid);

                return new AuthResponse
                {
                    Success = true,
                    Message = "Login bem-sucedido",
                    IdToken = idToken,
                    User = new UserInfo
                    {
                        Uid = userRecord.Uid,
                        Email = userRecord.Email,
                        DisplayName = userRecord.DisplayName,
                        EmailVerified = userRecord.EmailVerified
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer login do usuário");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                };
            }
        }
    }
}
