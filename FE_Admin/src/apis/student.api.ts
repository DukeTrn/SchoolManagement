import { IStudent, IStudentInfo } from "@/types/student.type";
import { ISuccessResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

interface IStudentBody {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
	status: number[];
}

export const getAllStudent = (body: IStudentBody) =>
	http.post<ISuccessResponseApi<IStudentInfo[]>>("student/all", body);

export const getStudentDetail = (id: string) =>
	http.get<{
		result: boolean;
		data: IStudent;
		messageType: number;
	}>(`student/${id}`);

export const createStudent = (body: FormData) => {
	return http.post("student/create", body, {
		headers: {
			"Content-Type": "multipart/form-data",
		},
	});
};

export const deleteStudent = (id: string) => http.delete(`student/${id}`);
