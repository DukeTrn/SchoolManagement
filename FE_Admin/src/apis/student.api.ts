import { IStudent, IStudentInfo } from "@/types/student.type";
import {
	ISuccessResponseApi,
	ISuccessGetResponseApi,
} from "@/types/utils.type";
import http from "@/utils/http";

interface IStudentBody {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
	status: number[];
}

interface IBodyStudentClassroom {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
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

export const updateStudent = (body: FormData, id: string) => {
	return http.put(`student/${id}`, body, {
		headers: {
			"Content-Type": "multipart/form-data",
		},
	});
};

export const deleteStudent = (id: string) => http.delete(`student/${id}`);

export const exportStudent = (body: {
	studentIds: string[];
	status: string[];
}) =>
	http.post("/student/export", body, {
		responseType: "blob",
	});

export const StudentFilterDetail = (id: string) =>
	http.get<ISuccessGetResponseApi<IStudent>>(`student/student/${id}`);

export const StudentClassDetail = (id: string) =>
	http.get<ISuccessGetResponseApi<IStudent>>(`student/student/${id}/classes`);

export const getStudentClassDetail = (
	body: IBodyStudentClassroom,
	classId: string
) => http.post<ISuccessResponseApi<any>>(`classdetail/all/${classId}`, body);
