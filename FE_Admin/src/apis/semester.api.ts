import { ISuccessResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

export const getAllSemesters = (body: {
	pageSize: number;
	pageNumber: number;
}) => http.post<ISuccessResponseApi<ISemester[]>>("semester/all", body);

export const createSemester = (body: ISemester) =>
	http.post("semester/create", body);

export const updateSemester = (body: ISemester) =>
	http.put(`semester/${body?.semesterId}`, body);

export const deleteSemester = (id: string) => http.delete(`semester/${id}`);
