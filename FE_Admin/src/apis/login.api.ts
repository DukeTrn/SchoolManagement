import http from "@/utils/http";

export const login = (body: { username: string; password: string }) =>
	http.post("user/login", body);
