using System.ComponentModel;

namespace Antiguera.Dominio.Enum
{
    public enum ETipoHistorico
    {
        [Description("Acesso")]
        Acesso = 1,
        [Description("Cadastro de jogo")]
        CadastroJogo = 2,
        [Description("Cadastro de programa")]
        CadastroPrograma = 3,
        [Description("Cadastro de emulador")]
        CadastroEmulador = 4,
        [Description("Cadastro de rom")]
        CadastroRom = 5,
        [Description("Adição de usuário")]
        InserirUsuario = 10,
        [Description("Atualização de usuário")]
        AtualizarUsuario = 11,
        [Description("Exclusão de usuário")]
        ExcluirUsuario = 12,
        [Description("Adição de jogo")]
        InserirJogo = 13,
        [Description("Atualização de jogo")]
        AtualizarJogo = 14,
        [Description("Exclusão de jogo")]
        ExcluirJogo = 15,
        [Description("Adição de programa")]
        InserirPrograma = 16,
        [Description("Atualização de programa")]
        AtualizarPrograma = 17,
        [Description("Exclusão de programa")]
        ExcluirPrograma = 18,
        [Description("Adição de emulador")]
        InserirEmulador = 19,
        [Description("Atualização de emulador")]
        AtualizarEmulador = 20,
        [Description("Exclusão de emulador")]
        ExcluirEmulador = 21,
        [Description("Adição de rom")]
        InserirRom = 22,
        [Description("Atualização de rom")]
        AtualizarRom = 23,
        [Description("Exclusão de rom")]
        ExcluirRom = 24,
        [Description("Adição de acesso")]
        InserirAcesso = 25,
        [Description("Atualização de acesso")]
        AtualizarAcesso = 26,
        [Description("Remoção de acesso")]
        ExcluirAcesso = 27,
        [Description("Atualização de senha")]
        AtualizarSenha = 28
    }
}