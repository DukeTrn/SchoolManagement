import {
	IAssignmentAdd,
	IAssignmentDisplay,
	ISubject,
} from "@/types/assignment.type";
import { ISuccessGetResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

interface ISubjectBody {
	subjectName: string;
	description: string;
	grade: number;
}

export const getSubjects = (grade: number) =>
	http.post<ISuccessGetResponseApi<ISubject[]>>(`subject/all/${grade}`);

export const createSubject = (body: ISubjectBody) =>
	http.post("subject/create", body);

export const updateSubject = (body: ISubjectBody, id: number) =>
	http.put(`subject/${id}`, body);

export const deleteSubject = (id: number) => http.delete(`subject/${id}`);

export const getStudentAssignment = (
	body: { searchValue: string },
	grade: number,
	semesterId: string,
	subjectId: number
) =>
	http.post<ISuccessGetResponseApi<IAssignmentDisplay[]>>(
		`assignment/all/${grade}/${semesterId}/${subjectId}`,
		body
	);

export const createAssignment = (body: IAssignmentAdd) =>
	http.post("assignment/create", body);

export const updateAssignment = (assignmentId: string) =>
	http.put(`assignment/update/${assignmentId}`);

export const deleteAssignment = (assignmentId: string) =>
	http.delete(`assignment/${assignmentId}`);
