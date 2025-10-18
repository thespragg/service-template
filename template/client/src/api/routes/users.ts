import type { User } from "@/types/api";
import { ApiClient } from "../ApiClient";

export class UsersEndpoint extends ApiClient {
  getCurrent() {
    return this.get<User>('/users/current');
  }
}
