export type IStudy = {
	classId: string;
	className: string;
	description: string;
	homeroomTeacherId?: string;
	homeroomTeacherName: string;
};

export type IClass = {
	studentId?: string;
	fullName: string;
	className: string;
	grade: string;
	academicYear: string;
};
