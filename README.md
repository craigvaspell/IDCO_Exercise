# IDCO_Exercise

## Assumptions

- Solution would only be exposed internally, e.g. in a cluster with no ingress routes defined, and naive implementation allows any internal services to consume.
	- Actual production solution would likely include some level of authorization to protect access, for example if only specific users/services are to access or if it was to be exposed publicly
<br><br>
- Input JSON represents account current/available balances as of date of requestDateTime, can back-calculate End-of-Day (eod) balances without needing to filter included transactions
- As accounts member in input JSON is an array but a single output report is desired then output report is an aggregate across all presented accounts
- Amounts in lowest denomination (e.g. pennies for GBP) so we don't need to deal with decimal place conversions

- Balances:
	- Input balances provided as absolute amounts (signage dictated by creditDebitIndicator)
	- Output total credits/debits provided as absolute amounts (signage dictated by member)
	- Output eod balances reported as signed amounts (no creditDebitIndicator in requested output schema)
	- Based on input JSON schema system could in theory cope with positive debits/negative credits but we're going to ignore this possibility for the output reporting as it's usually a bookkeeping measure rather than a property of the underlying transactions.

<br>

## Design Decisions

I'm assuming the brief is aimed at developing something that would suit in style/architecture to be easily plugged into an existing ecosystem, and given IDCO's existing platform I would probably say that's some sort of microservices or modular monolith type solution. With that in mind it's probably a good idea to aim for some sort of architecture with dependency inversion at its core, with a clearly defined domain at its heart and clearly defined interfaces at the extremities so we don't end up with unwanted coupling either within the balance reporting solution itself or between it and external components.

Domain will define entity models drawn from the provided input JSON and will be essentially a 1:1 mapping to the incoming DTO. While you could in theory consume the domain types directly in the API layer and break no rules, I think it's nicer to maintain that separation and have the controller responsible for translating between inbound/outbound types and that of the domain.

If at a future time the input/request DTO changes definition then it will be the responsibility of the ingest adapters to transform between that DTO and that of the domain models. Maybe the DTO changes are additions needed for some prevalidation and don't need to be propagated to the domain models so don't put the onus on the domain changing. Domain stays as is, adapters change. Excluding obvious case where the domain model itself is subject to change rather than just the transport representation we receive.

<br>

## Documentation

Solution has swagger generation included, located at "http(s)://\<applicationRoot\>/swagger" so as delivered: https://localhost:5001/swagger

<br>

## Dependencies

- AutoMapper
- Microsoft.AspNetCore.Mvc.NewtonsoftJson
- Microsoft.Extensions.Logging.Abstractions
- Moq
- Swashbuckle.AspNetCore
- Swashbuckle.AspNetCore.Newtonsoft
- XUnit

<br><br>

# Questions

### 1. What do you consider to be the most important skills for a software engineer? Why?

<br>

> Flexibility of thought and communication.
> 
> There's no such thing as a one-size-fits-all for either software or people: every situation, scenario, or solution is invariably an "it depends" and every person communicates in a slightly different way.
> 
> With that you need to be open to any train of thought; whether it's your own, from a peer who's had 30 odd years more experience, or the junior who just graduated yesterday. And you need to know how both yourself and your peers communicate to minimize things being lost in translation, and to keep people engaged. The further into my career I get, the more and more I find the balance shifting towards soft skills rather technical.

<br>

### 2. How does software engineering relate to other disciplines? - particularly engineering, crafts, and art.

<br>

> I think it depends on sector/industry, but I think it's probably a little bit of a blend between all the above. With a few notable exceptions such as in safety critical solutions (rockets, nuclear, medical, etc.) software engineering will never even begin to approach the rigor of process involved in engineering and will in comparison feel "cobbled together".
>
>There is a level of creative freedom in software that you won't see in engineering disciplines and even in the most strictly controlled and quality conscious of software development methodologies the king of quality assurance is still "break it until you find all the holes".

<br>

### 3. What do you think is the most under-rated tool, technique, or methodology in software engineering? What is the most over-rated?

<br>

> I don't know if it counts as a technique so much as an attitude but I think people really need to adopt a fail early approach. Everyone gets hit at some point with the design equivalent of decision paralysis, and I don't think you need to agonize over producing the perfect design before putting fingers to keys. Make a rough concept, fire out some tests, fulfill the implementation and see if it still suits your needs; if not then rinse and repeat.

<br>

### 4. What are your opinions on the current trends in software engineering? Do you think these trends are positive or negative?

<br>

> To be perfectly honest I'm not too clued up on current trends other than I guess there are more and more AI driven tools coming along such as Copilot. I think that one is a double eged sword; in the hands of someone that knows exactly what they're doing but wanting to save time then that's exactly what they'll get out of it, but in someone not so concerend about code quality I feel like that's going to lead to a proliferation of smelly code.
>
>But I don't know so much that what's trending is inherently good or bad, it's up to us as developers to choose to make use of something or not. That said I'm a little dubious of those pushing for a shift to plain procedural programming over OOP or functional solutions. That feels like the viewpoint of someone that hasn't had to *maintain* an enterprise solution.

<br>

### 5. What is your favourite programming language? Are there any features you would like to see removed? Added?

<br>

> I'd say I'm most at home in C#, there are so many features and sugar these days that you feel quite free to focus on the logic rather than the particulars of syntax.
>
> Things that I'd like added:
> - Covariant return type in interfaces
> - Discriminated unions, option and reult type along with the higher ordered functions you'd expect
>
>Things that I'd like removed:
> - Maybe the annoying edge cases that crop up with certain serializers/mapping utilities because of auto-emitted parameterless constructors, or primary constructors? I fully expect third party libraries to have issues but why does System.Text.Json have a hard time dealing with this?

<br>

### 6. Describe your process when faced with a challenging problem.

<br>

> If we're talking about a typical blocking issue, design or otherwise, then I'd probably do roughly the following:
> - Try to recall if I've encountered the same or similar before that I can draw upon
> - Curse my memory; I definitely have but can't remember the details
> - Try to find what I had previously come across, and either:
> 	- Success! I know how I can use this to solve the immediate problem
> <br>*or*<br>
> 	- Hmmmm, is there anyone I can reach out to that would be familiar with the subject or have some insight?
> 	- If not: cursory google/stackoverflow search
> 	- If not: time for some pen and paper to map out the problem
