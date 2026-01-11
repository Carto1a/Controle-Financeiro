import { ChevronDownIcon } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { useEffect, useState } from "react";

interface DatePickerProps {
  value?: Date;
  onChange?: (date: Date) => void;
  className?: string;
}

export function DatePicker(props: DatePickerProps) {
  const [open, setOpen] = useState(false);
  const [internalDate, setInternalDate] = useState<Date | undefined>(props.value);

  useEffect(() => {
    setInternalDate(props.value);
  }, [props.value]);

  const handleSelect = (date: Date) => {
    setInternalDate(date);
    props.onChange?.(date);
    setOpen(false);
  };

  return (
    <div className={`flex flex-col gap-1 ${props.className}`}>
      <Popover open={open} onOpenChange={setOpen}>
        <PopoverTrigger asChild>
          <Button
            variant="outline"
            className="w-48 justify-between font-normal"
          >
            {internalDate ? internalDate.toLocaleDateString() : "Selecione uma data"}
            <ChevronDownIcon className="ml-2 h-4 w-4" />
          </Button>
        </PopoverTrigger>
        <PopoverContent className="w-auto p-0 overflow-hidden" align="start">
          <Calendar
            required
            mode="single"
            selected={internalDate}
            captionLayout="dropdown"
            onSelect={handleSelect}
          />
        </PopoverContent>
      </Popover>
    </div>
  );
}
