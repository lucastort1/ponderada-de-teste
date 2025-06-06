# Funcionalidade: Login do Assessor

## Cenário: Login com credenciais válidas
&nbsp;Dado que o assessor forneceu email e senha corretos, quando o sistema processar o login, então o login deve ser realizado com sucesso e o token de autenticação deve ser retornado.

## Cenário: Login com senha incorreta
&nbsp;Dado que o assessor forneceu um email correto e uma senha incorreta, quando o sistema processar o login, então o login deve falhar e uma mensagem de erro deve ser exibida.

## Cenário: Erro interno ao processar login
&nbsp;Dado que uma falha de comunicação ocorra com o Firebase, quando o sistema tentar processar o login, então uma mensagem de erro interno deve ser retornada.