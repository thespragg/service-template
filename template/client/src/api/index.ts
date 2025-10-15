import { useAuth } from "@/composables/useAuth";
import { UsersEndpoint } from "./routes/users";

export class {{APP_NAME}}Api {
  public users: UsersEndpoint;

  constructor(baseUrl: string, getToken: () => string | null) {
    this.users = new UsersEndpoint(baseUrl, getToken);
  }
}

let apiInstance: {{APP_NAME}}Api | null = null;

export function use{{APP_NAME}}Api() {
  const { authToken } = useAuth();

  if (!apiInstance) {
    const baseUrl = import.meta.env.VITE_{{APP_NAME}}_API_URL;
    
    if (!baseUrl) {
      throw new Error('VITE_{{APP_NAME}}_API_URL is not defined in environment variables');
    }

    apiInstance = new {{APP_NAME}}Api(baseUrl, () => authToken.value);
  }

  return apiInstance;
}