import { ITeacher, ITeacherInfo } from "@/types/teacher.type";
import { ISuccessResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

interface ITeacherBody {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
	status: number[];
}

export const getAllTeacher = (body: ITeacherBody) =>
	http.post<ISuccessResponseApi<ITeacherInfo[]>>("teacher/all", body);

export const getTeacherDetail = (id: string) =>
	http.get<{
		result: boolean;
		data: ITeacher;
		messageType: number;
	}>(`teacher/${id}`);

export const createTeacher = (body: FormData) => {
	return http.post("teacher/create", body, {
		headers: {
			"Content-Type": "multipart/form-data",
		},
	});
};

export const updateTeacher = (body: FormData, id: string) => {
	return http.put(`teacher/${id}`, body, {
		headers: {
			"Content-Type": "multipart/form-data",
		},
	});
};

export const deleteTeacher = (id: string) => http.delete(`teacher/${id}`);

export const exportTeacher = (body: {
	teacherIds: string[];
	status: string[];
}) =>
	http.post("/teacher/export", body, {
		responseType: "blob",
	});
