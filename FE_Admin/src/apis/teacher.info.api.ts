import {
	ITeacherInfo,
	ITeacherCreateScore,
	ITeacherUpdateScore,
} from "@/types/teacher.info.type";
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

type ITeacherClassInfo = {
	pageSize: number;
	pageNumber: number;
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

export const getTeacherClass = (
	accountId: string,
	semesterId: string,
	body: ITeacherClassInfo
) =>
	http.post<ISuccessResponseApi<ITeacherInfo[]>>(
		`class/${accountId}/${semesterId}/teaching-classes`,
		body
	);

export const getTeacherStudentDetail = (
	grade: number,
	semesterId: string,
	classDetailId: string,
	subjectId: string
) =>
	http.post<ISuccessGetResponseApi<any>>(
		`assessment/${grade}/${semesterId}/${classDetailId}/${subjectId}/score`
	);

export const createTeacher = (body: ITeacherCreateScore) =>
	http.post("assessment/create", body);

export const updateTeacher = (body: ITeacherUpdateScore) =>
	http.put("assessment/update", body);
