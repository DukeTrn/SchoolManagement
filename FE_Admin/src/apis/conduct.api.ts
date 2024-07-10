import { ISuccessGetResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

interface IBodyClassroom {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
}

export const getConductInfo = (grade: number, semester: string) =>
	http.post<ISuccessGetResponseApi<[]>>(
		`conduct/${grade}/${semester}/classes`
	);

export const getConductClass = (
	body: IBodyClassroom,
	grade: number,
	semesterId: string,
	classId: string
) =>
	http.post<ISuccessGetResponseApi<any>>(
		`conduct/${grade}/${semesterId}/${classId}/students`,
		body
	);

export const updateConduct = (
	body: { conductType: number; feedback?: string },
	id: string
) => http.put(`conduct/${id}`, body);

export const getConductId = (studentId: string, semesterId: string) =>
	http.get<{
		conductId: string;
		conductName: number;
		feedback: string;
	}>(`conduct/student/${studentId}/semester/${semesterId}`);
