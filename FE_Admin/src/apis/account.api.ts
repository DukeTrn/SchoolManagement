import { IAccount } from "@/types/account.type";
import { ISuccessResponseApi, IUpdateResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

interface IAccountBody {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
	roles: number[];
}
export const getAllAccount = (body: IAccountBody) =>
	http.post<ISuccessResponseApi<IAccount[]>>("account/all", body);

export const activeAccount = (body: Pick<IAccount, "accountId" | "isActive">) =>
	http.put<IUpdateResponseApi>(
		`account/${body.accountId}/status/${body.isActive}`,
		body
	);

export const deleteAccount = (accountId: string) =>
	http.delete(`account/${accountId}`);
