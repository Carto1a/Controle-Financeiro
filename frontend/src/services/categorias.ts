import { api, type Paginated } from "../api/axios";

export const CategoriaFinalidade = {
  despesa: 0,
  receita: 1,
  ambas: 2,
} as const;
export type CategoriaFinalidade = typeof CategoriaFinalidade[keyof typeof CategoriaFinalidade];

export interface CriarCategoriaCommand {
  nome: string;
  descricao: string;
  finalidade: CategoriaFinalidade;
}

export interface Categoria {
  id: string;
  nome: string;
  descricao?: string;
  finalidade: CategoriaFinalidade;
}

export interface CategoriaSimples {
  id: string;
  nome: string;
}

export const categoriasService = {
  listar: async (): Promise<CategoriaSimples[]> => (await api.get('/categorias')).data,
  listarDetalhado: async (): Promise<Paginated<Categoria[]>> => (await api.get('/categorias/detalhado')).data,
  criar: async (data: CriarCategoriaCommand, signal: AbortSignal | undefined = undefined): Promise<void> =>
    (await api.post('/categorias', data, { signal: signal })).data,
};

