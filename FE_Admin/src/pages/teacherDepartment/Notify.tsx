import { getNotifyDepartment } from "@/apis/teacher.info.api";
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

export function Notify(props: IStudentDetailsProps) {
	const { id } = props;
	const [data, setData] = useState<string>();

	const getDetails = () => {
		getNotifyDepartment(id).then((res) => {
			setData(res?.data?.data);
		});
	};

	return (
		<Sheet>
			<SheetTrigger asChild>
				<Button variant="outline" onClick={getDetails}>
					Thông báo
				</Button>
			</SheetTrigger>
			<SheetContent className="max-h-screen overflow-y-scroll">
				<SheetHeader>
					<SheetTitle className="uppercase">
						Thông báo của tổ
					</SheetTitle>
				</SheetHeader>
				<div className="mt-6">
					<div className="mb-2">
						<div className=" font-medium">Thông báo</div>
					</div>
					<div className="mt-1">{data ? data : ""}</div>
				</div>
			</SheetContent>
		</Sheet>
	);
}
