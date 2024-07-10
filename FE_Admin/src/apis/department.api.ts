import { IDepartment } from "@/types/department.type";
import { ITeacher } from "@/types/teacher.type";
import { ISuccessResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

interface IDepartmentDetailBody {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
	status: number[];
}
type ITeacherTable = Pick<
	ITeacher,
	| "teacherId"
	| "fullName"
	| "dob"
	| "phoneNumber"
	| "email"
	| "level"
	| "status"
	| "gender"
>;
interface IAddTeacher {
	departmentId: string;
	teacherIds: string[];
}
interface IPromoteTeacher {
	departmentId: string;
	headId: string;
	firstDeputyId: string;
	secondDeputyId: string;
}
export const getAllDepartment = (body: {
	pageSize: number;
	pageNumber: number;
}) => http.post<ISuccessResponseApi<IDepartment[]>>("department/all", body);

export const createDepartment = (body: IDepartment) =>
	http.post("department/create", body);

export const updateDepartment = (body: IDepartment) =>
	http.put(`department/${body?.departmentId}`, body);

export const deleteDepartment = (id: string) => http.delete(`department/${id}`);

export const getDepartmentDetail = (body: IDepartmentDetailBody, id: string) =>
	http.post<ISuccessResponseApi<ITeacherTable[]>>(
		`department/${id}/teachers/all`,
		body
	);

export const getTeacherDepartment = () => http.get("department/teacher-filter");

export const addTeacherDepartment = (body: IAddTeacher) => {
	return http.post("department/add-teachers", body);
};

export const deleteTeacherDepartment = (body: IAddTeacher) => {
	return http.post("department/remove-teachers", body);
};

export const promoteTeacherDepartment = (body: IPromoteTeacher) => {
	return http.post("department/promote-teachers", body);
};
