import { IDepartment } from "@/types/department.type";
import { ISuccessResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

export const getAllDepartment = (body: {
	pageSize: number;
	pageNumber: number;
}) => http.post<ISuccessResponseApi<IDepartment[]>>("department/all", body);

export const createDepartment = (body: IDepartment) =>
	http.post("department/create", body);

export const updateDepartment = (body: IDepartment) =>
	http.put(`department/${body?.departmentId}`, body);

export const deleteDepartment = (id: string) => http.delete(`department/${id}`);
