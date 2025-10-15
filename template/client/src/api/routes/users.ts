import type { User } from "@/types/api";
import { ApiClient } from "../ApiClient";

export class UsersEndpoint extends ApiClient {
  getCurrent() {
    return this.get<User>('/user');
  }

  getById(id: string) {
    return this.get<User>(`/users/${id}`);
  }

  getAll() {
    return this.get<User[]>('/users');
  }

  update(id: string, data: Partial<User>) {
    return this.put<User>(`/users/${id}`, data);
  }

  delete(id: string): Promise<void>{
    return this.delete(`/users/${id}`);
  }
}
