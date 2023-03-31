import { Button, Grid } from "@mui/material";
import React from "react";

// props.content || props.content() // 
// props.next|prev|done?|select? = callbacks (the way item received is hidden)
// props.index, props.count
// props.cancel | cancel()

export default function Paginator(props){
    const range = { 
        from: 0,
        to: props.count - 1,
        
        [Symbol.iterator]() {
            this.current = this.from;
            return this;
        },

        next(){
            if (this.current <= this.to){
                return {done: false, value: this.current++ };
            } else {
                return {done: true}
            }
        }
    }

    return(<>
        <Button onClick={()=>typeof props.cancel === "function"?props.cancel(): alert(props.cancel)}>Back</Button>        
        <div>
            {typeof props.content === 'function'? props.content() : <>{props.content}</>}
        </div>
        <Button color='primary' disabled={props.index === 0} onClick={()=>props.prev}>Previous</Button>
        {props.done !== undefined && <Button color='success'>Done</Button>}
        <Button color='primary' disabled={props.count > props.index} onClick={()=>props.next}>Next</Button>
        {props.select && <Grid container spacing={1} justifyContent='center'>
        {() => {
            for (let r of range){
                return (<Grid item>
                    <Item onClick={() => props.select(r)}>{r + 1}</Item>
                </Grid>)
            }
        }}
        </Grid>}
    </>)
}