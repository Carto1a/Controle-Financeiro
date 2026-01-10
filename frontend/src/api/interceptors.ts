import { api } from "./axios";

export function setupInterceptors() {
  api.interceptors.request.use(config => {
    return config;
  });

  api.interceptors.response.use(
    r => r,
    error => {
      return Promise.reject(error);
    }
  );
}
