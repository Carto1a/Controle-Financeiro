import { api } from "../api/axios";

export interface CriarPessoaCommand {
  nome?: string;
  dataNascimento: string;
}

export interface Pessoa {
  id: string;
  nome: string;
  dataNascimento: string;
}

export interface PessoaSimples {
  id: string;
  nome: string;
}

export const pessoaService = {
  listar: async (): Promise<PessoaSimples[]> => (await api.get('/pessoa')).data,
  listarDetalhado: async (): Promise<Pessoa[]> => (await api.get('/pessoa/detalhado')).data,
  criar: async (data: CriarPessoaCommand): Promise<void> =>
    (await api.post('/pessoa', data)).data,
  deletar: async (id: string): Promise<void> =>
    (await api.delete(`/pessoa/${id}`)).data,
};

