import { ITeacherInfo, ITeacherClass } from "@/types/teacher.info.type";
import {
	ISuccessGetResponseApi,
	ISuccessResponseApi,
} from "@/types/utils.type";
import http from "@/utils/http";

type ITeacherInfoBody = {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
	status: number[];
};

type IHeadOfDepartmentInfo = {
	role: number;
	teacherId: string;
	fullName: string;
};

export const getTeacherDepartmentInfo = (body: ITeacherInfoBody, id: string) =>
	http.post<ISuccessResponseApi<ITeacherInfo[]>>(
		`department/${id}/teachers`,
		body
	);

export const getHeadOfDepartment = (departmentId: string) =>
	http.get<ISuccessGetResponseApi<IHeadOfDepartmentInfo[]>>(
		`department/${departmentId}/teachers/heads`
	);

export const getNotifyDepartment = (departmentId: string) =>
	http.get<ISuccessGetResponseApi<string>>(
		`department/${departmentId}/notification`
	);
