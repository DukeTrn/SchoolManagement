import { IClassroom, IStudentClassroom } from "@/types/classroom.type";

import {
	ISuccessGetResponseApi,
	ISuccessResponseApi,
} from "@/types/utils.type";
import http from "@/utils/http";

interface IBodyClassroom {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
}
type IClassDetailBody = IBodyClassroom & { status: number[] };
type IBodyAddCLassroom = Omit<IClassroom, "classId" | "homeroomTeacherName">;

export const getClassroom = (body: IBodyClassroom, id: number) =>
	http.post<ISuccessResponseApi<IClassroom[]>>(`class/all/${id}`, body);

export const getFreeTeacher = (academicYear: string) =>
	http.get<ISuccessGetResponseApi<{ teacherId: string; fullName: string }[]>>(
		`class/teacher-filter/${encodeURIComponent(academicYear)}`
	);

export const createClassroom = (body: IBodyAddCLassroom) =>
	http.post("class/create", body);

export const deleteClass = (id: string) => http.delete(`class/${id}`);

export const updateClass = (
	body: { className: string; hoomroomTeacherId?: string },
	id: string,
	academicYear: string
) => http.put(`class/${id}?academicYear=${academicYear}`, body);

export const getClassDetail = (body: IClassDetailBody, id: string) =>
	http.post<ISuccessResponseApi<IStudentClassroom[]>>(
		`classdetail/all/${id}`,
		body
	);

export const getFreeStudent = (params: {
	academicYear: string;
	grade: number;
}) =>
	http.get("classdetail/filter-students", {
		params: params,
	});

export const addStudentToClass = (
	body: {
		classId: string;
		studentId: string;
	}[]
) => http.post("classdetail/create", body);

export const getClassFilter = (academicYear: string, grade: number) =>
	http.get<ISuccessGetResponseApi<{ classId: string; className: string }[]>>(
		`class/class-filter/${encodeURIComponent(academicYear)}/${grade}`
	);

export const updateStudentClass = (classDetailId: string, newClassId: string) =>
	http.put(`classdetail/${classDetailId}/${newClassId}`);

export const deleteClassDetail = (id: string) =>
	http.delete(`classdetail/${id}`);
