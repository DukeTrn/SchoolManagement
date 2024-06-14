import { getTeacherDetail } from "@/apis/teacher.api";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { ITeacher } from "@/types/teacher.type";
import { useState } from "react";

interface IStudentDetailsProps {
	teacherId: string;
}

export function TeacherDetails(props: IStudentDetailsProps) {
	const { teacherId } = props;
	const [teacher, setTeacher] = useState<ITeacher>();

	const getStudentDetails = () => {
		getTeacherDetail(teacherId).then((res) => {
			setTeacher(res?.data?.data);
		});
	};

	return (
		<Sheet>
			<SheetTrigger asChild>
				<div
					className="cursor-pointer font-medium text-blue-600 underline"
					onClick={getStudentDetails}
				>
					{teacherId}
				</div>
			</SheetTrigger>
			<SheetContent className="max-h-screen overflow-y-scroll">
				<SheetHeader>
					<SheetTitle className="uppercase">
						Chi tiết giáo viên
					</SheetTitle>
				</SheetHeader>
				<div className="mt-6">
					<div className="mb-6 flex items-center justify-center">
						<Avatar className="h-28 w-28">
							<AvatarImage src={teacher?.avatar} alt="@shadcn" />
							<AvatarFallback>AVT</AvatarFallback>
						</Avatar>
					</div>
					<div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Mã số giáo viên:
							</span>
							<span>{teacher?.teacherId}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Họ tên giáo viên:
							</span>
							<span>{teacher?.fullName}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Ngày sinh:</span>
							<span>{teacher?.dob}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Giới tính:</span>
							<span>{teacher?.gender}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">CCCD:</span>
							<span>{teacher?.identificationNumber}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Dân tộc:</span>
							<span>{teacher?.ethnic}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Địa chỉ:</span>
							<span>{teacher?.address}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Số điện thoại:
							</span>
							<span>{teacher?.phoneNumber}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Email:</span>
							<span>{teacher?.email}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Trình độ:</span>
							<span>{teacher?.level}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Tình trạng giảng dạy:
							</span>
							<span>{teacher?.status}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Ngày bắt đầu:
							</span>
							<span>{teacher?.timeStart}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Ngày kết thúc:
							</span>
							<span>{teacher?.timeEnd}</span>
						</div>
					</div>
				</div>
			</SheetContent>
		</Sheet>
	);
}
