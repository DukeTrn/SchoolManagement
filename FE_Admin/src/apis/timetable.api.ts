import { ISubject } from "@/types/assignment.type";
import { ITimetable } from "@/types/timetable.type";
import { ISuccessGetResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

type ICreateTimetable = {
	classId: string;
	semesterId: string;
	subjectId: number;
	dayOfWeek: number;
	period: number;
};
export const getTimetableList = (classId: string, semesterId: string) =>
	http.get<ITimetable[]>(`class/timetable/${classId}/${semesterId}/list`);

export const getSubjectOfGrade = (grade: number) =>
	http.post<ISuccessGetResponseApi<ISubject[]>>(`subject/all/${grade}`);

export const createTimetable = (body: ICreateTimetable) =>
	http.post("class/timetable/create", body);

export const getTimetableDetail = (timetableId: string) =>
	http.get<ISuccessGetResponseApi<ITimetable>>(
		`class/timetable/${timetableId}`
	);

export const updateTimetable = (
	body: Pick<ICreateTimetable, "period" | "dayOfWeek">,
	timetableId: string
) => http.put(`class/timetable/${timetableId}`, body);

export const removeTimetable = (timetableId: string) =>
	http.delete(`class/timetable/${timetableId}`);

export const getTeacherTimetableList = (
	accountId: string,
	semesterId: string
) =>
	http.get<ITimetable[]>(
		`class/timetable/teacher/${accountId}/${semesterId}/list`
	);
