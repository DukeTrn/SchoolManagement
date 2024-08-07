import { ISuccessResponseApi } from "@/types/utils.type";
import http from "@/utils/http";
export const registerAccount = (body: { email: string; password: string }) =>
	http.post<ISuccessResponseApi<string>>("/register", body);

export const logoutAccount = () => http.post("/logout");

export const changePassword = (
	body: {
		oldPassword: string;
		newPassword: string;
		confirmPassword: string;
	},
	accountId: string
) => http.put(`account/${accountId}/changepassword`, body);
