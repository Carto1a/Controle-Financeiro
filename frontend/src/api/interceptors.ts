import toast from "react-hot-toast";
import { api } from "./axios";
import { HttpStatusCode, type AxiosError } from "axios";

export function setupInterceptors() {
  api.interceptors.request.use(config => {
    return config;
  });

  api.interceptors.response.use(
    response => response,
    (error: AxiosError<any>) => {
      if (!error.response) {
        toast.error("Erro de conexão com o servidor")
        return Promise.reject(error)
      }

      const status = error.response.status
      const data = error.response.data as any

      if (status === 400 || status === 422) {
        return Promise.reject(error)
      }

      if ([
        HttpStatusCode.MethodNotAllowed,
        HttpStatusCode.NotFound,
        HttpStatusCode.NotImplemented].includes(status)) {
        toast.error("Erro de conexão com o servidor")
        return Promise.reject()
      }

      toast.error(
        data?.message ??
        "Ocorreu um erro inesperado. Tente novamente."
      )

      return Promise.reject(error)
    }
  );
}
