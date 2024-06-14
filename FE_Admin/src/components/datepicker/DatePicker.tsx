import { format } from "date-fns";
import { Calendar as CalendarIcon } from "lucide-react";

import { cn } from "@/lib/utils";
import { Popover, PopoverContent, PopoverTrigger } from "../ui/popover";
import { Button } from "../ui/button";
import { Calendar } from "../ui/calendar";

interface IProps {
	date: any;
	setDate: any;
	errorMessage?: string;
}

export default function DatePicker(props: IProps) {
	const { date, setDate, errorMessage } = props;

	return (
		<Popover>
			<PopoverTrigger asChild>
				<Button
					variant={"outline"}
					className={cn(
						"w-full min-w-[280px] justify-start text-left font-normal",
						!date && "text-muted-foreground"
					)}
				>
					<CalendarIcon className="mr-2 h-4 w-4" />
					{date ? format(date, "PPP") : <span>Chọn thời gian</span>}
				</Button>
			</PopoverTrigger>
			<PopoverContent className="w-auto p-0">
				<Calendar
					mode="single"
					selected={date}
					onSelect={setDate}
					initialFocus
					captionLayout="dropdown-buttons"
					fromYear={1900}
					toYear={new Date().getFullYear()}
				/>
			</PopoverContent>
			<div className="mt-1 min-h-[1.25rem] text-sm text-red-600">
				{errorMessage}
			</div>
		</Popover>
	);
}
