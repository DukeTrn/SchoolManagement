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
type IBodyAddCLassroom = Omit<IClassroom, "classId" | "homeroomTeacherName">;

export const getClassroom = (body: IBodyClassroom, id: number) =>
	http.post<ISuccessResponseApi<IClassroom[]>>(`class/all/${id}`, body);

export const getFreeTeacher = (academicYear: string, grade: string) =>
	http.get<ISuccessGetResponseApi<{ teacherId: string; fullName: string }[]>>(
		`class/teacher-filter/${encodeURIComponent(academicYear)}/${grade}`
	);

export const getClassFilter = (academicYear: string, grade: string) =>
	http.get<ISuccessGetResponseApi<{ classId: string; className: string }[]>>(
		`class/class-filter/${encodeURIComponent(academicYear)}/${grade}`
	);

export const createClassroom = (body: IBodyAddCLassroom) =>
	http.post("class/create", body);

export const deleteClass = (id: string) => http.delete(`class/${id}`);

export const updateClass = (
	body: { className: string; hoomroomTeacherId?: string },
	id: string
) => http.put(`class/${id}`, body);
