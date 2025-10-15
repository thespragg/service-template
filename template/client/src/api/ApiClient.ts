type HttpMethod = 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';

interface RequestConfig {
  method?: HttpMethod;
  body?: unknown;
  headers?: Record<string, string>;
}

export class ApiClient {
  constructor(
    private baseUrl: string,
    private getToken: () => string | null
  ) {}

  private async request<T>(endpoint: string, config: RequestConfig = {}): Promise<T> {
    const { method = 'GET', body, headers = {} } = config;
    const token = this.getToken();

    const response = await fetch(`${this.baseUrl}${endpoint}`, {
      method,
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` }),
        ...headers,
      },
      body: body ? JSON.stringify(body) : undefined,
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`API Error: ${response.status} ${response.statusText} - ${errorText}`);
    }

    return response.json();
  }

  protected get<T>(endpoint: string, headers?: Record<string, string>): Promise<T> {
    return this.request<T>(endpoint, { method: 'GET', headers });
  }

  protected post<T>(endpoint: string, body?: unknown, headers?: Record<string, string>): Promise<T> {
    return this.request<T>(endpoint, { method: 'POST', body, headers });
  }

  protected put<T>(endpoint: string, body?: unknown, headers?: Record<string, string>): Promise<T> {
    return this.request<T>(endpoint, { method: 'PUT', body, headers });
  }

  protected patch<T>(endpoint: string, body?: unknown, headers?: Record<string, string>): Promise<T> {
    return this.request<T>(endpoint, { method: 'PATCH', body, headers });
  }

  protected delete(endpoint: string, headers?: Record<string, string>): Promise<void> {
    return this.request(endpoint, { method: 'DELETE', headers });
  }
}
