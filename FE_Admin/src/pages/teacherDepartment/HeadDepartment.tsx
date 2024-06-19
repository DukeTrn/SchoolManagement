import { getTeacherDetail } from "@/apis/teacher.api";
import { getHeadOfDepartment } from "@/apis/teacher.info.api";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { useState } from "react";

interface IStudentDetailsProps {
	id: string;
}
type IHeadOfDepartmentInfo = {
	role: number;
	teacherId: string;
	fullName: string;
};

export function HeadDepartment(props: IStudentDetailsProps) {
	const { id } = props;
	const [data, setData] = useState<IHeadOfDepartmentInfo[]>();

	const getDetails = () => {
		getHeadOfDepartment(id).then((res) => {
			setData(res?.data?.data);
		});
	};

	return (
		<Sheet>
			<SheetTrigger asChild>
				<Button variant="outline" onClick={getDetails}>
					Trưởng/Phó bộ môn
				</Button>
			</SheetTrigger>
			<SheetContent className="max-h-screen overflow-y-scroll">
				<SheetHeader>
					<SheetTitle className="uppercase">
						Trưởng/phó bộ môn
					</SheetTitle>
				</SheetHeader>
				<div className="mt-6">
					<div className="mb-4 flex">
						<div className="w-[150px] font-medium">
							Trưởng bộ môn:
						</div>
						<div>{data?.[0]?.fullName}</div>
					</div>
					<div className="mb-4 flex">
						<div className="w-[150px] font-medium">
							Phó bộ môn 1:
						</div>
						<div>{data?.[1]?.fullName}</div>
					</div>
					<div className="mb-4 flex">
						<div className="w-[150px] font-medium">
							Phó bộ môn 2:
						</div>
						<div>{data?.[2]?.fullName}</div>
					</div>
				</div>
			</SheetContent>
		</Sheet>
	);
}
